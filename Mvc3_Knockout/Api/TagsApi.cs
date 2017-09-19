using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Dispatcher;
using BasketballPlaybook.Models;
//using BasketballPlaybook.Resources;

namespace BasketballPlaybook
{ 
	[ServiceContract]
    public class TagsApi
    {
		private readonly ITagRepository tagRepository;


		public TagsApi (ITagRepository tagRepository) 
		{
			this.tagRepository = tagRepository;
		}

        [WebGet]
        public HttpResponseMessage<List<Tag>> GetAll()
        {
            var list = this.tagRepository.All.ToList();
            var response = new HttpResponseMessage<List<Tag>>(list);
            return response;
        }

        [WebGet(UriTemplate = "{id}")]
        public HttpResponseMessage<Tag> Get(int id)
        {
			var item = this.tagRepository.Find(id);
			if (item == null)
			{
                var notFoundResponse = new HttpResponseMessage();
                notFoundResponse.StatusCode = HttpStatusCode.NotFound;
                notFoundResponse.Content = new StringContent("Item not found");
                throw new HttpResponseException(notFoundResponse);
            }
            var response = new HttpResponseMessage<Tag>(item);

            // set it to expire in 5 minutes
            response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(30));
            return response;
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public HttpResponseMessage<Tag> Post(Tag item)
        {
			this.tagRepository.InsertOrUpdate(item);
            this.tagRepository.Save();

            var response = new HttpResponseMessage<Tag>(item);
            response.StatusCode = HttpStatusCode.Created;
            response.Headers.Location = new Uri("tags/" + item.Id, UriKind.Relative);
            return response;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public Tag Put(int id, Tag item)
        {
			this.tagRepository.InsertOrUpdate(item);
            this.tagRepository.Save();
            return item;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public HttpResponseMessage Delete(int id)
        {
			this.tagRepository.Delete(id);
            this.tagRepository.Save();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
 
	}
}

