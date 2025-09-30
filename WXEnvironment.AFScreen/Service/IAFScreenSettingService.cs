using MongoDB.Driver;
using WXEnvironment.Common;

namespace WXEnvironment.AFScreen.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAFScreenSettingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result<Data.AFScreenSettingModel>> Get(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        public Task<Result<Data.AFScreenSettingModel>> GetByInfoId(string infoId);

        /**/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sessionMongo"></param>
        /// <returns></returns>
        public Task<Result<bool>> Insert(Data.AFScreenSettingModel model, IClientSessionHandle? sessionMongo = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="model"></param>
        /// <param name="sessionMongo"></param>
        /// <returns></returns>
        public Task<Result<bool>> DeleteAndCreate(string infoId, Data.AFScreenSettingModel model, IClientSessionHandle? sessionMongo = null);

        /**/

        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<Result<List<Data.AFScreenSettingModel>>> Page(Data.QueryAFScreenSetting query);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<Result<List<Data.AFScreenSettingModel>>> List(Data.QueryAFScreenSetting query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionMongo"></param>
        /// <returns></returns>
        public Task<Result<bool>> Delete(string id, IClientSessionHandle? sessionMongo = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="sessionMongo"></param>
        /// <returns></returns>
        public Task<Result<bool>> Delete(List<string> ids, IClientSessionHandle? sessionMongo = null);
    }
}
