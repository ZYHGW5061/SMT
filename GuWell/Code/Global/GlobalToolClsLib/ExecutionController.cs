using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalToolClsLib
{
    public class ExecutionController
    {
        // 单例实例
        private static readonly Lazy<ExecutionController> _instance =
            new Lazy<ExecutionController>(() => new ExecutionController());

        public static ExecutionController Instance => _instance.Value;

        // 核心同步对象
        private readonly EventWaitHandle _waitHandle =
            new EventWaitHandle(false, EventResetMode.AutoReset);

        // 控制状态
        private volatile bool _isPaused;
        public bool IsPaused => _isPaused;

        private ExecutionController() { }

        // 暂停执行
        public void Pause() => _isPaused = true;

        // 继续执行
        public void Continue()
        {
            _isPaused = false;
            _waitHandle.Set();  // 释放所有等待线程
            DataModel.Instance.SysRunSta = GlobalDataDefineClsLib.EnumProductRunStat.AutoRun;
        }

        // 单步执行
        public void Step()
        {
            _isPaused = true;   // 保持暂停状态
            _waitHandle.Set();  // 允许执行一个操作
            DataModel.Instance.SysRunSta = GlobalDataDefineClsLib.EnumProductRunStat.AutoPause;
        }

        // 等待控制点
        public void WaitIfPaused()
        {
            if (!_isPaused) return;
            DataModel.Instance.SysRunSta = GlobalDataDefineClsLib.EnumProductRunStat.AutoPause;
            // 阻塞直到收到继续/单步信号
            _waitHandle.WaitOne();
        }
    }
}
