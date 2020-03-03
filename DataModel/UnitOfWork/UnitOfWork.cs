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
        private GenericRepository<Hotel> _HotelRepository;
        private GenericRepository<Status> _StatusRepository;
        private GenericRepository<Floor> _FloorRepository;
        private GenericRepository<ServiceType> _ServiceTypeRepository;
        private GenericRepository<Service> _ServiceRepository;
        private GenericRepository<User> _UserRepository;
        private GenericRepository<Room> _RoomRepository;
        private GenericRepository<Guest> _GuestRepository;



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

        public GenericRepository<Hotel> HotelRepository
        {
            get
            {
                if (this._HotelRepository == null)
                    this._HotelRepository = new GenericRepository<Hotel>(_context);
                return _HotelRepository;
            }
        }
        public GenericRepository<Status> StatusRepository
        {
            get
            {
                if (this._StatusRepository == null)
                    this._StatusRepository = new GenericRepository<Status>(_context);
                return _StatusRepository;
            }
        }
        public GenericRepository<Floor> FloorRepository
        {
            get
            {
                if (this._FloorRepository == null)
                    this._FloorRepository = new GenericRepository<Floor>(_context);
                return _FloorRepository;
            }
        }

        public GenericRepository<ServiceType> ServiceTypeRepository
        {
            get
            {
                if (this._ServiceTypeRepository == null)
                    this._ServiceTypeRepository = new GenericRepository<ServiceType>(_context);
                return _ServiceTypeRepository;
            }
        }
        public GenericRepository<Service> ServiceRepository
        {
            get
            {
                if (this._ServiceRepository == null)
                    this._ServiceRepository = new GenericRepository<Service>(_context);
                return _ServiceRepository;
            }
        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._UserRepository == null)
                    this._UserRepository = new GenericRepository<User>(_context);
                return _UserRepository;
            }
        }
        public GenericRepository<Room> RoomRepository
        {
            get
            {
                if (this._RoomRepository == null)
                    this._RoomRepository = new GenericRepository<Room>(_context);
                return _RoomRepository;
            }
        }
        public GenericRepository<Guest> GuestRepository
        {
            get
            {
                if (this._GuestRepository == null)
                    this._GuestRepository = new GenericRepository<Guest>(_context);
                return _GuestRepository;
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
