using PrismMetroSample.Infrastructure.Interceptor.HandlerAttributes;
using PrismMetroSample.Infrastructure.Models;
using System.Collections.Generic;

namespace PrismMetroSample.Infrastructure.Services
{
    [LogHandler]
   public interface IMedicineSerivce
    {
        List<Medicine> GetAllMedicines();

        List<Recipe> GetRecipesByPatientId(int patientId);
    }
}
