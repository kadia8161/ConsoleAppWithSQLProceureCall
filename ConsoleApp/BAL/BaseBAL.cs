using ConsoleApp.Model;
using System;
namespace ConsoleApp.BAL
{
    public abstract class BaseBAL<T> : IDisposable
    {
        public EcommContext ctx { get; set; } 
        protected abstract bool AddData(T model);
        protected abstract bool UpdateData(T model);
        public abstract bool DeleteData(int Id);
        protected int SaveData()
        {
            return ctx.SaveChanges();
        }
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}