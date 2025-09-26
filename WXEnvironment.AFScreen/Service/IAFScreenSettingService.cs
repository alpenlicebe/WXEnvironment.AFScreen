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
        public Task<Result<bool>> UpdateInfo(string infoId, Data.AFScreenSettingModel model, IClientSessionHandle? sessionMongo = null);

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

        
    }
}
