//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace wlhx.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExperimentTime
    {
        public long experimentTime_id { get; set; }
        public string experimentTime_startTime { get; set; }
        public string experimentTime_week { get; set; }
        public long experimentTime_ownExperimentId { get; set; }
        public bool experimentTime_isDel { get; set; }
        public int experimentTime_peopleNum { get; set; }
    
        public virtual Experiment Experiment { get; set; }
    }
}