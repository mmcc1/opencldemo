using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLTemplateExample
{
    public struct Values
    {
        public float[] input1;
        public float[] input2;
        public float[] output;
    }

    public class OpenCLPrograms
    {
        public Values Sum()
        {
            Values val = new Values();
            string vecSum = @"
                     __kernel void
floatVectorSum(__global       float * v1,
__global       float * v2)
{
// Vector element index
int i = get_global_id(0);
v1[i] = v1[i] + v2[i];
}";
            //List<Cloo.ComputeDevice> cd = OpenCLTemplate.CLCalc.CLDevices;
            //OpenCLTemplate.CLCalc.Program.DefaultCQ = cd.ElementAt(0).Handle;
            

            OpenCLTemplate.CLCalc.InitCL(Cloo.ComputeDeviceTypes.Cpu);  //Select where we will execute our program

            //Compiles the source codes. The source is a string array because the user may want
            //to split the source into many strings.
            OpenCLTemplate.CLCalc.Program.Compile(new string[] { vecSum });

            //Gets host access to the OpenCL floatVectorSum kernel
            OpenCLTemplate.CLCalc.Program.Kernel VectorSum = new OpenCLTemplate.CLCalc.Program.Kernel("floatVectorSum");

            //We want to sum 2000 numbers
            int n = 2000;

            //Create vectors with 2000 numbers
            val.input1 = new float[n];
            val.input2 = new float[n];
            val.output = new float[n];

            //Creates population for v1 and v2
            for (int i = 0; i < n; i++)
            {
                val.input1[i] = (float)i / 10;
                val.input2[i] = -(float)i / 9;
            }

            //Creates vectors v1 and v2 in the device memory
            OpenCLTemplate.CLCalc.Program.Variable varV1 = new OpenCLTemplate.CLCalc.Program.Variable(val.input1);
            OpenCLTemplate.CLCalc.Program.Variable varV2 = new OpenCLTemplate.CLCalc.Program.Variable(val.input2);

            //Arguments of VectorSum kernel
            OpenCLTemplate.CLCalc.Program.Variable[] args = new OpenCLTemplate.CLCalc.Program.Variable[] { varV1, varV2 };

            //How many workers will there be? We need “n”, one for each element
            int[] workers = new int[1] { n };

            //Execute the kernel
            VectorSum.Execute(args, workers);

            //Read device memory varV1 to host memory v1
            varV1.ReadFromDeviceTo(val.output);

            return val;
        }
    }
}
