using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamadaDAL
{
   public abstract class DALBase : IDisposable
    {

        protected ValidConnection cn = ValidConnection.Cn;
        public void Dispose()
        {
            if (cn.Trans == null)
            {
                cn.Dispose();
                cn = null;
            }
        }

    }
}
