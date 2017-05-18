using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ledgerdemo.Service {
    public class DisplayedException : Exception {
        public DisplayedException(string message) : base(message) { }
    }
}
