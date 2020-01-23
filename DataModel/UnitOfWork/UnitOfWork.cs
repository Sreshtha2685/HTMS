#region Using Namespaces...

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data;
using DataModel;
using DataModel.GenericReository;

using Country = DataModel.Country;

#endregion

namespace DataModel.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private HTMEntities3 _context = null;
        //private GenericRepository<LoginUser> _LoginUserRepository;


        // ---------------------------HTMS------------------------------------------------
        private GenericRepository<Country> _CountryRepository;
        private GenericRepository<RoomType> _RoomTypeRepository;
        private GenericRepository<State> _StateRepository;
        private GenericRepository<City> _CityRepository;
        private GenericRepository<Bed> _BedRepository;


        #endregion

        public UnitOfWork()
        {
            _context = new HTMEntities3();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for city repository.
        /// </summary>
        /// 
        public GenericRepository<Country> CountryRepository
        {
            get
            {
                if (this._CountryRepository == null)
                    this._CountryRepository = new GenericRepository<Country>(_context);
                return _CountryRepository;
            }
        }


        public GenericRepository<RoomType> RoomTypeRepository
        {
            get
            {
                if (this._RoomTypeRepository == null)
                    this._RoomTypeRepository = new GenericRepository<RoomType>(_context);
                return _RoomTypeRepository;
            }
        }
        public GenericRepository<Bed> BedRepository
        {
            get
            {
                if (this._BedRepository == null)
                    this._BedRepository = new GenericRepository<Bed>(_context);
                return _BedRepository;
            }
        }

        public GenericRepository<State> StateRepository
        {
            get
            {
                if (this._StateRepository == null)
                    this._StateRepository = new GenericRepository<State>(_context);
                return _StateRepository;
            }
        }
        public GenericRepository<City> CityRepository
        {
            get
            {
                if (this._CityRepository == null)
                    this._CityRepository = new GenericRepository<City>(_context);
                return _CityRepository;
            }
        }

        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
