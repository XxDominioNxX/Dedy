using Dedy.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MongoApp.Models
{
    public class ProductService
    {
        IGridFSBucket gridFS;   // файловое хранилище
        IMongoCollection<Product> Products; // коллекция в базе данных
        public ProductService()
        {
            // строка подключения
            string connectionString = "mongodb://localhost:27017/Products";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
            // обращаемся к коллекции Products
            Products = database.GetCollection<Product>("Products");
        }
        // получаем все документы, используя критерии фальтрации
        public async Task<IEnumerable<Product>> GetProducts()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Empty; // фильтр для выборки всех документов       

            return await Products.Find(filter).ToListAsync();
        }

        public IEnumerable<Product> GetProducts(int Page, int PageSize)
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Empty; // фильтр для выборки всех документов       

            return Products.Find(filter).ToList().GetRange(((Page - 1) * PageSize), PageSize);
        }

        public IEnumerable<Product> GetProducts(int Page, int PageSize, FilterDefinition<Product> filt)
        { 
            
            return Products.Find(filt).ToList().GetRange(((Page - 1) * PageSize), PageSize);
        }



        public IEnumerable<Product> GetProductsList()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Empty; // фильтр для выборки всех документов       

            return Products.Find(filter).ToList();
        }








        // получаем один документ по id
        public async Task<Product> GetProduct(string id)
        {
            return await Products.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(Product p)
        {
            await Products.InsertOneAsync(p);
        }
        // обновление документа
        public async Task Update(Product p)
        {
            await Products.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }
        // удаление документа
        public async Task Remove(string id)
        {
            await Products.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        // получение изображения
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }
        // сохранение изображения
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            Product p = await GetProduct(id);
            
            // сохраняем изображение
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            // обновляем данные по документу
            p.ImageId = imageId.ToString();
            var filter = Builders<Product>.Filter.Eq("_id", new ObjectId(p.Id));
            var update = Builders<Product>.Update.Set("ImageId", p.ImageId);
            await Products.UpdateOneAsync(filter, update);
        }

        public string GetCount()
        { 
            return Products.AsQueryable<Product>().ToList().Count.ToString();
        }

        public string GetCount(FilterDefinition<Product> filt)
        {
            return Products.Find(filt).ToList().Count.ToString();
        }
    }
}