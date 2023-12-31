﻿namespace Services.Storage
{
    public interface IStorage<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
