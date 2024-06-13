using Blazor.Infrastructure.Entities;
using Dominus.Backend.Application;
using Dominus.Backend.DataBase;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blazor.BusinessLogic
{
    public class ArchivosBusinessLogic : GenericBusinessLogic<Archivos>
    {
        public ArchivosBusinessLogic(IUnitOfWork unitWork) : base(unitWork)
        {
        }

        public ArchivosBusinessLogic(DataBaseSetting configuracionBD) : base(configuracionBD)
        {
        }

        public override Archivos FindById(Expression<Func<Archivos, bool>> predicate, bool include = false)
        {
            var data = base.FindById(predicate, include);
            if (data != null)
            {
                data.StringToBase64 = DApp.Util.ArrayBytesToString(data.Archivo);
            }
            return data;
        }

        public override List<Archivos> FindAll(Expression<Func<Archivos, bool>> predicate, bool include = false)
        {
            var datas = base.FindAll(predicate, include);
            datas.ForEach(x => {
                if (x != null)
                {
                    x.StringToBase64 = DApp.Util.ArrayBytesToString(x.Archivo);
                }
            });
            return datas;
        }

    }
}

