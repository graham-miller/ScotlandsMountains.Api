using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ScotlandsMountains.Api.Loader.Resources
{
    public class HillCsvReader : IReader
    {
        private readonly ResourceStream _resourceStream;
        private readonly ZipArchive _zipArchive;
        private readonly Stream _csvStream;
        private readonly StreamReader _reader;
        private readonly CsvReader _csv;

        public HillCsvReader()
        {
            _resourceStream = new ResourceStream(FileNames.HillCsvZip);
            _zipArchive = new ZipArchive(_resourceStream);
            _csvStream = _zipArchive.Entries[0].Open();
            _reader = new StreamReader(_csvStream);
            _csv = new CsvReader(_reader, CultureInfo.InvariantCulture);
        }

        public string GetField(int index)
        {
            return _csv.GetField(index);
        }

        public string GetField(string name)
        {
            return _csv.GetField(name);
        }

        public string GetField(string name, int index)
        {
            return _csv.GetField(name, index);
        }

        public object GetField(Type type, int index)
        {
            return _csv.GetField(type, index);
        }

        public object GetField(Type type, string name)
        {
            return _csv.GetField(type, name);
        }

        public object GetField(Type type, string name, int index)
        {
            return _csv.GetField(type, name, index);
        }

        public object GetField(Type type, int index, ITypeConverter converter)
        {
            return _csv.GetField(type, index, converter);
        }

        public object GetField(Type type, string name, ITypeConverter converter)
        {
            return _csv.GetField(type, name, converter);
        }

        public object GetField(Type type, string name, int index, ITypeConverter converter)
        {
            return _csv.GetField(type, name, index, converter);
        }

        public T GetField<T>(int index)
        {
            return _csv.GetField<T>(index);
        }

        public T GetField<T>(string name)
        {
            return _csv.GetField<T>(name);
        }

        public T GetField<T>(string name, int index)
        {
            return _csv.GetField<T>(name, index);
        }

        public T GetField<T>(int index, ITypeConverter converter)
        {
            return _csv.GetField<T>(index, converter);
        }

        public T GetField<T>(string name, ITypeConverter converter)
        {
            return _csv.GetField<T>(name, converter);
        }

        public T GetField<T>(string name, int index, ITypeConverter converter)
        {
            return _csv.GetField<T>(name, index, converter);
        }

        public T GetField<T, TConverter>(int index) where TConverter : ITypeConverter
        {
            return _csv.GetField<T, TConverter>(index);
        }

        public T GetField<T, TConverter>(string name) where TConverter : ITypeConverter
        {
            return _csv.GetField<T, TConverter>(name);
        }

        public T GetField<T, TConverter>(string name, int index) where TConverter : ITypeConverter
        {
            return _csv.GetField<T, TConverter>(name, index);
        }

        public bool TryGetField(Type type, int index, out object field)
        {
            return _csv.TryGetField(type, index, out field);
        }

        public bool TryGetField(Type type, string name, out object field)
        {
            return _csv.TryGetField(type, name, out field);
        }

        public bool TryGetField(Type type, string name, int index, out object field)
        {
            return _csv.TryGetField(type, name, index, out field);
        }

        public bool TryGetField(Type type, int index, ITypeConverter converter, out object field)
        {
            return _csv.TryGetField(type, index, converter, out field);
        }

        public bool TryGetField(Type type, string name, ITypeConverter converter, out object field)
        {
            return _csv.TryGetField(type, name, converter, out field);
        }

        public bool TryGetField(Type type, string name, int index, ITypeConverter converter, out object field)
        {
            return _csv.TryGetField(type, name, index, converter, out field);
        }

        public bool TryGetField<T>(int index, out T field)
        {
            return _csv.TryGetField(index, out field);
        }

        public bool TryGetField<T>(string name, out T field)
        {
            return _csv.TryGetField(name, out field);
        }

        public bool TryGetField<T>(string name, int index, out T field)
        {
            return _csv.TryGetField(name, index, out field);
        }

        public bool TryGetField<T>(int index, ITypeConverter converter, out T field)
        {
            return _csv.TryGetField(index, converter, out field);
        }

        public bool TryGetField<T>(string name, ITypeConverter converter, out T field)
        {
            return _csv.TryGetField(name, converter, out field);
        }

        public bool TryGetField<T>(string name, int index, ITypeConverter converter, out T field)
        {
            return _csv.TryGetField(name, index, converter, out field);
        }

        public bool TryGetField<T, TConverter>(int index, out T field) where TConverter : ITypeConverter
        {
            return _csv.TryGetField(index, out field);
        }

        public bool TryGetField<T, TConverter>(string name, out T field) where TConverter : ITypeConverter
        {
            return _csv.TryGetField(name, out field);
        }

        public bool TryGetField<T, TConverter>(string name, int index, out T field) where TConverter : ITypeConverter
        {
            return _csv.TryGetField(name, index, out field);
        }

        public T GetRecord<T>()
        {
            return _csv.GetRecord<T>();
        }

        public T GetRecord<T>(T anonymousTypeDefinition)
        {
            return _csv.GetRecord(anonymousTypeDefinition);
        }

        public object GetRecord(Type type)
        {
            return _csv.GetRecord(type);
        }

        public int ColumnCount => _csv.ColumnCount;

        public int CurrentIndex => _csv.CurrentIndex;

        public string[] HeaderRecord => _csv.HeaderRecord;

        public IParser Parser => _csv.Parser;

        public CsvContext Context => _csv.Context;

        public IReaderConfiguration Configuration => _csv.Configuration;

        public string this[int index] => _csv[index];

        public string this[string name] => _csv[name];

        public string this[string name, int index] => _csv[name, index];

        public bool ReadHeader()
        {
            return _csv.ReadHeader();
        }

        public bool Read()
        {
            return _csv.Read();
        }

        public Task<bool> ReadAsync()
        {
            return _csv.ReadAsync();
        }

        public IEnumerable<T> GetRecords<T>()
        {
            return _csv.GetRecords<T>();
        }

        public IEnumerable<T> GetRecords<T>(T anonymousTypeDefinition)
        {
            return _csv.GetRecords(anonymousTypeDefinition);
        }

        public IEnumerable<object> GetRecords(Type type)
        {
            return _csv.GetRecords(type);
        }

        public IEnumerable<T> EnumerateRecords<T>(T record)
        {
            return _csv.EnumerateRecords(record);
        }

        public IAsyncEnumerable<T> GetRecordsAsync<T>(CancellationToken cancellationToken = new CancellationToken())
        {
            return _csv.GetRecordsAsync<T>(cancellationToken);
        }

        public IAsyncEnumerable<T> GetRecordsAsync<T>(T anonymousTypeDefinition,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _csv.GetRecordsAsync(anonymousTypeDefinition, cancellationToken);
        }

        public IAsyncEnumerable<object> GetRecordsAsync(Type type, CancellationToken cancellationToken = new CancellationToken())
        {
            return _csv.GetRecordsAsync(type, cancellationToken);
        }

        public IAsyncEnumerable<T> EnumerateRecordsAsync<T>(T record, CancellationToken cancellationToken = new CancellationToken())
        {
            return _csv.EnumerateRecordsAsync(record, cancellationToken);
        }

        public void Dispose()
        {
            _csv.Dispose();
            _reader.Dispose();
            _csvStream.Dispose();
            _zipArchive.Dispose();
            _resourceStream.Dispose();
        }
    }
}
