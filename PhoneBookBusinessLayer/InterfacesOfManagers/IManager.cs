﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PhoneBookEntityLayer.ResultModels;
using PhoneBookEntityLayer.ViewModels;

namespace PhoneBookBusinessLayer.InterfacesOfManagers
{
    public interface IManager<T,Id>
    {
        IDataResult<T> Add(T model); //DataResult ile eklenen verinin ıdsine ihtiyac duyarsak geriye dönen datadan ıd yi alabiliri.
        IResult Update(T model);
        IResult Delete(T model);

        IDataResult<ICollection<T>> GetAll(Expression<Func<T,bool>>? filter = null);
        IDataResult<T> GetByConditions(Expression<Func<T, bool>>? filter = null);
        IDataResult<T> GetById(Id id);


    }
}
