using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;
using WXEnvironment.Auth.Service;
using WXEnvironment.Common;
using WXEnvironment.AFScreen.Data;
using WXEnvironment.AFScreen.Service;
using WXEnvironment.MongoDB;

#pragma warning disable CS8602 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8619 // Possible null reference argument.


namespace WXEnvironment.AFScreen.Dao
{
    /// <summary>
    /// 设备
    /// </summary>
    public class DeviceDao : BaseDao<Data.AFScreenSettingModel>, IAFScreenSettingService
    {
        private readonly IValidator<Data.AFScreenSettingModel> _validator;

        private readonly IAccountService _accountService;
        private readonly IPlatformFieldService _platformFieldService;
        private readonly ICurrentUserInfoService _user;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoClient"></param>
        /// <param name="validator"></param>
        /// <param name="accountService"></param>
        /// <param name="platformFieldService"></param>
        /// <param name="user"></param>
        public DeviceDao(IMongoClient mongoClient, IValidator<Data.AFScreenSettingModel> validator, IAccountService accountService, IPlatformFieldService platformFieldService, ICurrentUserInfoService user)
            : base(mongoClient, "wx_af", "setting")
        {
            _validator = validator;

            _accountService = accountService;
            _platformFieldService = platformFieldService;
            _user = user;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {

        }

        /// <summary>
        /// 单条
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        public async Task<Result<Data.AFScreenSettingModel>> GetByInfoId(string infoId)
        {
            if (string.IsNullOrEmpty(infoId))
                return Result<Data.AFScreenSettingModel>.NotOk("参数为空");
            var builder = Builders<Data.AFScreenSettingModel>.Filter;
            var filter = builder.And(
                builder.Eq(c => c.FlagDelete, false),
                builder.Eq(c => c.InfoId, infoId));
            return await this.MongoGetOne(filter);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sessionMongo">注意：如果传递了session，请务必自行进行session的CommitTransaction</param>
        /// <returns></returns>
        public async Task<Result<bool>> Insert(Data.AFScreenSettingModel model, IClientSessionHandle? sessionMongo = null)
        {
            //
            model.Id = ObjectId.GenerateNewId().ToString();
            model.CreateAccount = _user.AccountId;
            model.CreateAccountName = _user.AccountName;
            model.ModifyAccount = "";
            model.ModifyAccountName = "";
            model.ModifyTime = null;
            model.FlagDelete = false;
            model.DeleteAccount = "";
            model.DeleteAccountName = "";
            model.DeleteTime = null;

            var v = _validator.Validate(model);
            if (!v.IsValid)
                return Result<bool>.NotOk(v.ToString(";"));

            var tmp = await this.GetByInfoId(model.InfoId);
            if (tmp.ok && tmp.value != null)
                return Result<bool>.NotOk("“InfoId”已存在，请修改后重试");

            var acc = await _accountService.Get(model.BelongUserId);
            if (!acc.ok || acc.value == null)
                return Result<bool>.NotOk("“用户”不存在，请修改后重试");
            model.BelongUserName = acc.value.Name;

            if (sessionMongo == null)
            {
                using (var session = this.MongoClient.StartSession())
                {
                    try
                    {
                        session.StartTransaction();
                        var result = await this.MongoInsertOne(session, model);
                        if (!result.ok)
                            throw new Exception(result.message);
                        session.CommitTransaction();
                        return Result<bool>.Ok(true, model.Id);
                    }
                    catch (Exception ex)
                    {
                        session.AbortTransaction();
                        return Result<bool>.NotOk(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    var result = await this.MongoInsertOne(sessionMongo, model);
                    if (!result.ok)
                        throw new Exception(result.message);
                    return Result<bool>.Ok(true, model.Id);
                }
                catch (Exception ex)
                {
                    throw new Exception("操作失败：" + ex.Message);
                }
            }
        }

        #region 更新单条
        private async Task<Result<bool>> UpdateOneAlpenliebe(FilterDefinition<Data.AFScreenSettingModel> filter, UpdateDefinition<Data.AFScreenSettingModel> update, UpdateOptions? options = null, IClientSessionHandle? sessionMongo = null)
        {
            if (sessionMongo == null)
            {
                using (var session = this.MongoClient.StartSession())
                {
                    try
                    {
                        session.StartTransaction();
                        var result = await this.MongoUpdateOne(session, filter, update, options);
                        if (!result.ok)
                            throw new Exception("updateex操作失败：" + result.message);
                        session.CommitTransaction();
                        if (result.value == null)
                            return Result<bool>.NotOk("未更新任何数据");
                        return Result<bool>.Ok(true, 1);
                    }
                    catch (Exception ex)
                    {
                        session.AbortTransaction();
                        return Result<bool>.NotOk(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    var result = await this.MongoUpdateOne(sessionMongo, filter, update, options);
                    if (!result.ok)
                        throw new Exception("updateex操作失败：" + result.message);
                    if (result.value == null)
                        return Result<bool>.NotOk("未更新任何数据");
                    return Result<bool>.Ok(true, 1);
                }
                catch (Exception ex)
                {
                    throw new Exception("操作失败：" + ex.Message);
                }
            }
        }
        #endregion


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="model"></param>
        /// <param name="sessionMongo">注意：如果传递了session，请务必自行进行session的CommitTransaction</param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateInfo(string infoId, Data.AFScreenSettingModel model, IClientSessionHandle? sessionMongo = null)
        {
            if (string.IsNullOrEmpty(infoId))
                return Result<bool>.NotOk("参数为空");
            var data = await this.GetByInfoId(infoId);
            if (!data.ok || data.value == null)
                return Result<bool>.NotOk("数据不存在");

            var builder = Builders<Data.AFScreenSettingModel>.Filter;
            var filter = builder.And(builder.Eq(c => c.DocVersion, data.value.DocVersion), builder.Eq(c => c.InfoId, infoId));
            var update = Builders<Data.AFScreenSettingModel>.Update
                .Set(c => c.ModifyAccount, _user.AccountId)
                .Set(c => c.ModifyAccountName, _user.AccountName)
                .Set(c => c.ModifyTime, DateTime.Now);
            update = update
                .SetIfNotNull(c => c.BgType, model.BgType)
                .SetIfNotNull(c => c.BgColor, model.BgColor)
                .SetIfNotNull(c => c.BgImage, model.BgImage)

                .SetIfNotNull(c => c.ActiveElementId, model.ActiveElementId)
                
                .SetIfNotNull(c => c.Sort, model.Sort)
                .SetIfNotNull(c => c.Note, model.Note)
                .SetIfNotNull(c => c.Extend, model.Extend);

            return await this.UpdateOneAlpenliebe(filter, update, new UpdateOptions() { IsUpsert = false }, sessionMongo);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Result<List<Data.AFScreenSettingModel>>> Page(Data.QueryAFScreenSetting query)
        {
            var sort = Builders<Data.AFScreenSettingModel>.Sort.Descending(c => c.Sort).Descending(c => c.CreateTime);
            return await this.MongoQuery(query, sort);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<Data.AFScreenSettingModel>>> List(Data.QueryAFScreenSetting query)
        {
            var sort = Builders<Data.AFScreenSettingModel>.Sort.Descending(c => c.Sort).Descending(c => c.CreateTime);
            var data = await this.MongoQueryCursor(query, sort, new FindOptions() { BatchSize = query.PageSize });
            if (!data.ok)
                return Result<List<Data.AFScreenSettingModel>>.NotOk(data.message);
            var list = new List<Data.AFScreenSettingModel>();
            using (data.value)
            {
                while (data.value.MoveNext())
                {
                    var tmp = data.value.Current;
                    foreach (var item in tmp)
                    {
                        list.Add(item);
                    }
                }
            }
            return Result<List<Data.AFScreenSettingModel>>.Ok(list, data.ext);
        }
    }
}
