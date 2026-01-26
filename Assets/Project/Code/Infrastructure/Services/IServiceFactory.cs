using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code.Infrastructure.Services
{
    public interface IServiceFactory
    {
        T CreateService<T>() where T : IService;
    }
}
