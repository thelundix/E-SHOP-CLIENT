using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnys.api.Services;

public interface IOrderService
{
    string GenerateOrderNumber();
}
