using Azure;
using System.Reflection;
using System.Text.RegularExpressions;
using UniwayBackend.Factories;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Repositories.Core.Interfaces;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        private readonly ILogger<ReviewService> _logger;
        private readonly UtilitariesResponse<Review> _utilitaries;
        private readonly UtilitariesResponse<ReviewSummaryResponse> _utilitaries1;

        public ReviewService(IReviewRepository repository, ILogger<ReviewService> logger, UtilitariesResponse<Review> utilitaries, UtilitariesResponse<ReviewSummaryResponse> utilitaries1)
        {
            _repository = repository;
            _logger = logger;
            _utilitaries = utilitaries;
            _utilitaries1 = utilitaries1;
        }

        public async Task<MessageResponse<Review>> GetAllByTechnical(int TechnicalId)
        {
            MessageResponse<Review> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var reviews = await _repository.FindAllByTechnicalId(TechnicalId);

                response = _utilitaries.setResponseBaseForList(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<ReviewSummaryResponse>> GetSummaryByTechnical(int TechnicalId)
        {
            MessageResponse<ReviewSummaryResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var reviews = await _repository.FindAllByTechnicalId(TechnicalId);

                // Calcular el promedio de estrellas
                double averageStarRating = reviews.Average(x => x.StarNumber);

                // Encontrar la palabra más común en los títulos
                var titles = reviews.Select(x => x.Title).Where(x => !string.IsNullOrEmpty(x));
                var words = titles
                    .SelectMany(title => Regex.Matches(title.ToLower(), @"\b[\w']+\b"))
                    .Select(match => match.Value)
                    .Where(word => word.Length > 3) // Ignorar palabras cortas
                    .GroupBy(word => word)
                    .OrderByDescending(group => group.Count())
                    .ThenBy(group => group.Key)
                    .FirstOrDefault();

                var summary = new ReviewSummaryResponse
                {
                    TechnicalId = TechnicalId,
                    AverageStartNumber = averageStarRating,
                    WorkdKey = words?.Key ?? "N/A",
                };

                response = _utilitaries1.setResponseBaseForObject(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries1.setResponseBaseForException(ex);
            }
            return response;
        }

        public async Task<MessageResponse<Review>> Save(Review model)
        {
            MessageResponse<Review> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);
                model.ReviewDate = DateTime.Now;

                var review = await _repository.InsertAndReturn(model);

                response = _utilitaries.setResponseBaseForObject(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = _utilitaries.setResponseBaseForException(ex);
            }
            return response;
        }
    }
}
