using PhoneStore.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Services.IServices
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllAsync();
    }
}
