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

namespace BasketballPlaybook
{ 
	[ServiceContract]
    public class DrillsApi
    {
		private readonly IDrillRepository drillRepository;

		public DrillsApi (IDrillRepository drillRepository) 
		{
			this.drillRepository = drillRepository;
		}

        [WebGet(UriTemplate = "?tagId={tagId}")]
        public HttpResponseMessage<List<Drill>> GetByTag(int tagId)
        {
            var list = this.drillRepository.All.Where(d => d.TagId == tagId).ToList();
            var response = new HttpResponseMessage<List<Drill>>(list);
            return response;
        }

        [WebGet(UriTemplate = "{id}")]
        public HttpResponseMessage<Drill> Get(int id)
        {
			var item = this.drillRepository.Find(id);
			if (item == null)
			{
                var notFoundResponse = new HttpResponseMessage();
                notFoundResponse.StatusCode = HttpStatusCode.NotFound;
                notFoundResponse.Content = new StringContent("Item not found");
                throw new HttpResponseException(notFoundResponse);
            }
            var response = new HttpResponseMessage<Drill>(item);

            // set it to expire in 5 minutes
            response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(30));
            return response;
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public HttpResponseMessage<Drill> Post(Drill item)
        {
			this.drillRepository.InsertOrUpdate(item);
            this.drillRepository.Save();

            var response = new HttpResponseMessage<Drill>(item);
            response.StatusCode = HttpStatusCode.Created;
            response.Headers.Location = new Uri("drills/" + item.Id, UriKind.Relative);
            return response;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public Drill Put(int id, Drill item)
        {
			this.drillRepository.InsertOrUpdate(item);
            this.drillRepository.Save();
            return item;
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public HttpResponseMessage Delete(int id)
        {
			this.drillRepository.Delete(id);
            this.drillRepository.Save();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
 
	}
}

