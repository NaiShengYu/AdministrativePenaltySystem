using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace Sample

{

    public interface IToast

    {

        void LongAlert(string message);

        void ShortAlert(string message);

    }

}