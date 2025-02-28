using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Abstractions
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdf(string content);
    }
}
