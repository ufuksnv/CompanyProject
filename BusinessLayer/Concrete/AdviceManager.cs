using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AdviceManager : IAdviceService
    {
        IAdviceDal _adviceDal;

        public AdviceManager(IAdviceDal adviceDal)
        {
            _adviceDal = adviceDal;
        }

        public void TAdd(Advice t)
        {
            _adviceDal.Insert(t);
        }

        public void TDelete(Advice t)
        {
            _adviceDal.Delete(t);
        }

        public Advice TGetByID(int id)
        {
            return _adviceDal.GetByID(id);
        }

        public List<Advice> TGetList()
        {
            return _adviceDal.GetList();
        }

        public void TUpdate(Advice t)
        {
            _adviceDal.Update(t);
        }
    }
}
