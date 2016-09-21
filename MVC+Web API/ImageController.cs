using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MVC_Web_API
{
    public class ImageController : ApiController
    {
        #region LastUploadedSrc
        private static string _lastUploadedSrc = string.Empty;

        /// <summary>
        /// In case if HasUpdate() returns true, 
        /// ajax gonna call this method and take last uploaded (and saved) img src. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult LastUploadedSrc()
        {
            var justCopy = _lastUploadedSrc;
            _lastUploadedSrc = string.Empty;
            return Json(justCopy);
        }
        #endregion

        /// <summary>
        /// Every 2 seconds ajax gonna ask this method, if something new has came. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult HasUpdate() => Json(_lastUploadedSrc != string.Empty);

        /// <summary>
        /// This method is responsible only for one thing: it saves the image,
        /// given as byte array, and saves it to the Images folder. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            var imgFileAsByteArr = await Request.Content.ReadAsByteArrayAsync();

            _lastUploadedSrc = 
                $"Images/{Request.Headers.First(h => h.Key == "fileName").Value.First()}";

            File.WriteAllBytes(
                HttpContext.Current.Server.MapPath("~/" + _lastUploadedSrc), 
                imgFileAsByteArr);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
