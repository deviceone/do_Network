using doCore.Helper.JsonParse;
using doCore.Interface;
using doCore.Object;
using do_Network.extdefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace do_Network.extimplement
{
    /// <summary>
    /// 自定义扩展API组件Model实现，继承DoSingletonModule抽象类，并实现@TYPEID_IMethod接口方法；
    /// #如何调用组件自定义事件？可以通过如下方法触发事件：
    /// this.model.getEventCenter().fireEvent(_messageName, jsonResult);
    /// 参数解释：@_messageName字符串事件名称，@jsonResult传递事件参数对象；
    /// 获取DoInvokeResult对象方式this.model.getCurrentPage().getScriptEngine().createInvokeResult(model.getUniqueKey());
    /// </summary>
    public class do_Network_Model : doSingletonModule, do_Network_IMethod
    {
        public do_Network_Model():base()
        {
        }

        /// <summary>
        /// 同步方法，JS脚本调用该组件对象方法时会被调用，可以根据_methodName调用相应的接口实现方法；
        /// </summary>
        /// <param name="_methodName">方法名称</param>
        /// <param name="_dictParas">参数（K,V）</param>
        /// <param name="_scriptEngine">当前Page JS上下文环境对象</param>
        /// <param name="_invokeResult">用于返回方法结果对象</param>
        /// <returns></returns>
        public override bool InvokeSyncMethod(string _methodName, doCore.Helper.JsonParse.doJsonNode _dictParas, doCore.Interface.doIScriptEngine _scriptEngine, doInvokeResult _invokeResult)
        {
			if ("getIP".Equals(_methodName)){this.getIP(_dictParas, _scriptEngine, _invokeResult);return true;}
			if ("getOperators".Equals(_methodName)){this.getOperators(_dictParas, _scriptEngine, _invokeResult);return true;}
			if ("getStatus".Equals(_methodName)){this.getStatus(_dictParas, _scriptEngine, _invokeResult);return true;}

            return base.InvokeSyncMethod(_methodName, _dictParas, _scriptEngine, _invokeResult);
        }
		private void getIP(doJsonNode _dictParas, doIScriptEngine _scriptEngine, doInvokeResult _invokeResult){
            ConnectionProfile conf = NetworkInformation.GetInternetConnectionProfile();
            if (conf == null)
            {
                _invokeResult.SetResultText("none");
            }
            else
            {
                if (conf.IsWlanConnectionProfile)
                {
                    _invokeResult.SetResultText("wifi");
                }
                else if (conf.IsWwanConnectionProfile)
                {
                    _invokeResult.SetResultText("2g/3g");
                }
                else
                {
                    _invokeResult.SetResultText("unknow");
                }
            }
                
        }
		private void getOperators(doJsonNode _dictParas, doIScriptEngine _scriptEngine, doInvokeResult _invokeResult){
            ConnectionProfile conf = NetworkInformation.GetInternetConnectionProfile();
            var guid = conf.ServiceProviderGuid;
            if(guid==null)
                _invokeResult.SetResultText("没有手机卡");
            _invokeResult.SetResultText(guid.ToString());
        }
		private void getStatus(doJsonNode _dictParas, doIScriptEngine _scriptEngine, doInvokeResult _invokeResult){
            var hostn=NetworkInformation.GetHostNames();
            var hostname = hostn.Where(t => t.Type == Windows.Networking.HostNameType.Ipv4).FirstOrDefault();
            _invokeResult.SetResultText(hostname.DisplayName);   
        }

        /// <summary>
        /// 异步方法（通常都处理些耗时操作，避免UI线程阻塞），JS脚本调用该组件对象方法时会被调用，
        /// 可以根据_methodName调用相应的接口实现方法；
        /// </summary>
        /// <param name="_methodName">方法名称</param>
        /// <param name="_dictParas">参数（K,V）</param>
        /// <param name="_scriptEngine">当前page JS上下文环境</param>
        /// <param name="_callbackFuncName">回调函数名</param>
        /// <returns></returns>
        public override async Task<bool> InvokeAsyncMethod(string _methodName, doCore.Helper.JsonParse.doJsonNode _dictParas, doCore.Interface.doIScriptEngine _scriptEngine, string _callbackFuncName)
        {
            doInvokeResult _invokeResult = new doInvokeResult(this.UniqueKey);

            return await base.InvokeAsyncMethod(_methodName, _dictParas, _scriptEngine, _callbackFuncName);
        }
    
    }
}
