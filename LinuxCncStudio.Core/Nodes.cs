using System;
using System.Collections.Generic;
namespace LinuxCncStudio.Core.Nodes;

public enum PinDataType { Bit, Float, S32, U32 }
public enum PinIoType { Input, Output, IO }

public class VisualPin {
    public string Name { get; set; } = string.Empty;
    public PinDataType DataType { get; set; }
    public PinIoType IoType { get; set; }
    public string Description { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }
}

public abstract class BaseNode {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public double CanvasX { get; set; }
    public double CanvasY { get; set; }
    public List<VisualPin> Inputs { get; set; } = new();
    public List<VisualPin> Outputs { get; set; } = new();
}

public class MotionNode : BaseNode {
    public MotionNode() {
        Title = "LinuxCNC Motion Core";
        Outputs.Add(new VisualPin { Name = "axis.0.motor-pos-cmd", DataType = PinDataType.Float, IoType = PinIoType.Output });
        Inputs.Add(new VisualPin { Name = "motion.motion-enabled", DataType = PinDataType.Bit, IoType = PinIoType.Input });
    }
}

public class StepgenNode : BaseNode {
    public StepgenNode() {
        Title = "Software Stepgen (Axis X)";
        Inputs.Add(new VisualPin { Name = "stepgen.0.position-cmd", DataType = PinDataType.Float, IoType = PinIoType.Input });
        Outputs.Add(new VisualPin { Name = "stepgen.0.velocity-fb", DataType = PinDataType.Float, IoType = PinIoType.Output });
    }
}
