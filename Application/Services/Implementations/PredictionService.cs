using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Creates;
using Domain.Models.Systems;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;


namespace Application.Services.Implementations
{
    public class PredictionService : BaseService, IPredictionService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IPlantRepository _plantRepository;
        private readonly IClassRepository _classRepository;
        private readonly new IMapper _mapper;

        private const double MinimumRate = 0.7;

        public PredictionService(IUnitOfWork unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper)
        {
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
            _plantRepository = unitOfWork.Plant;
            _classRepository = unitOfWork.Class;
        }

        private async Task<string> ExecuteCommandAsync(string workingDirectory, string command, string arguments)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardInput = true
                };

                process.StartInfo = startInfo;

                StringBuilder terminalOutput = new StringBuilder();

                process.OutputDataReceived += (sender, e) => Console.WriteLine("Service: " + e.Data);

                process.ErrorDataReceived += (sender, e) =>
                {
                    Console.WriteLine("Python: " + e.Data);
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        terminalOutput.AppendLine(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                process.Close();

                return terminalOutput.ToString();
            }
        }

        private async Task<string> GetImageUrl(Guid id, IFormFile image)
        {
            try
            {
                return await _cloudStorageService.Upload(id, image.ContentType, image.OpenReadStream());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TerminalResult> GetResult(string terminalOutput)
        {

            // Sử dụng regular expressions để trích xuất các giá trị
            string pattern = @"(?<label>\w+) (?<confidence>[\d.]+),";
            MatchCollection matches = Regex.Matches(terminalOutput, pattern);

            // Tạo danh sách các đối tượng Result
            List<TerminalResult> results = new List<TerminalResult>();
            foreach (Match match in matches)
            {
                TerminalResult result = new TerminalResult
                {
                    Label = match.Groups["label"].Value,
                    Confidence = double.Parse(match.Groups["confidence"].Value)
                };
                results.Add(result);
            }
            return results;
        }

        public async Task<IActionResult> ProgressImage(IFormFile image)
        {
            try
            {
                var imageId = Guid.NewGuid();
                string workingDirectory = @"C:\Users\Janglee\Desktop\plant_classfify";
                var imageUrl = await GetImageUrl(imageId, image);
                string command = "py";
                string arguments = "main.py " + imageUrl;

                var output = await ExecuteCommandAsync(workingDirectory, command, arguments);

                var terminalResult = GetResult(output);

                var estimate = await GetEstimateResult(terminalResult);

                var finalData = new ResultViewModel
                {
                    CreateAt = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Estimate = estimate,
                };

                var bestResult = terminalResult[0];

                if (bestResult != null && bestResult.Confidence > MinimumRate)
                {
                    var plant = await GetPlant(bestResult.Label);
                    finalData.Plant = plant;
                }

                return new ObjectResult(finalData)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ProgressImageForClass(Guid classId, IFormFile image)
        {
            try
            {
                var imageId = Guid.NewGuid();
                var classCode = _classRepository.GetMany(cl => cl.Id.Equals(classId)).Select(cl => cl.Code).FirstOrDefault();

                if (classCode == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                string workingDirectory = @"C:\Users\Janglee\Desktop\plant_classfify";
                string classDirectory = workingDirectory + "\\" + classCode;
                var imageUrl = await GetImageUrl(imageId, image);
                string command = "py";
                string arguments = "main.py " + imageUrl + " " + classDirectory;

                var output = await ExecuteCommandAsync(workingDirectory, command, arguments);

                var terminalResult = GetResult(output);

                var estimate = await GetEstimateResult(terminalResult);

                var finalData = new ResultViewModel
                {
                    CreateAt = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Estimate = estimate,
                };

                var bestResult = terminalResult[0];

                if (bestResult != null && bestResult.Confidence > MinimumRate)
                {
                    var plant = await GetPlant(bestResult.Label);
                    finalData.Plant = plant;
                }

                return new ObjectResult(finalData)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<PlantViewModel> GetPlant(string code)
        {
            try
            {
                var plant = await _plantRepository.GetMany(pl => pl.Code.Equals(code))
                    .ProjectTo<PlantViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return plant != null ? plant : null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<EstimateViewModel>> GetEstimateResult(List<TerminalResult> results)
        {
            try
            {
                List<EstimateViewModel> estimates = new List<EstimateViewModel>();
                foreach (var result in results)
                {
                    var plant = await _plantRepository.GetMany(pl => pl.Code.Equals(result.Label))
                        .ProjectTo<PlantViewModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                    if (plant != null)
                    {
                        var estimate = new EstimateViewModel
                        {
                            Plant = plant,
                            Confidence = result.Confidence,
                        };
                        estimates.Add(estimate);
                    }
                }
                return estimates;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult UpdateClassModel(Guid classId, ClassModelCreateModel model)
        {
            try
            {
                var file = model.Model;
                var classCode = _classRepository.GetMany(cl => cl.Id.Equals(classId)).Select(cl => cl.Code).FirstOrDefault();

                if (classCode == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                string folderPath = @"C:\Users\Janglee\Desktop\plant_classfify\classes\" + classCode;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, file.FileName);

                // Ghi file vào đường dẫn đã tạo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return new ObjectResult("File saved successfully")
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}