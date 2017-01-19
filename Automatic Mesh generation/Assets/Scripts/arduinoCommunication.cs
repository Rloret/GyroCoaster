using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Threading;

public static class arduinoCommunication
{

    private static SerialPort stream;
    //private bool noData = true;

    public static float[] inputData = { 0, 0, 0, 0, 0, 0 };

    public static int createAndOpenPort(int baudRate)
    {
        /* if (stream != null) stream.Close();
         stream = new SerialPort(port, baudRate);
         stream.Open();
         stream.ReadTimeout = 1;

         return 1;*/
        int result = -1; ;
        string[] ports = SerialPort.GetPortNames();
        foreach (string po in ports)
        {
            Debug.Log(po);
            try {
                stream = new SerialPort("\\\\.\\"+po, baudRate);
                stream.Open();
                stream.ReadTimeout = 1;
                result= 1;
            }
            catch (Exception e)
            {

                Debug.LogError("el error es " +e.GetType());
                closePort();
                result = -1;
                //throw;
                
            }

        }
        return result;

    }

    private static void parseString(string s)
    {
        int counter = 0;
        int auxcounter = 0;
        float[] values = { 0,0,0,0,0,0 };
        string valueAux = "";

        for (int i = 0; i < s.Length; i++)

        {
            if (s[i] == '#')
            {
                auxcounter++;
            }
        }
        //Debug.Log(s);
        if (auxcounter == 6)
        {
            for (int i = 0; i < s.Length; i++)

            {
                if (s[i] == '#')
                {
                    if (valueAux == "")
                    {
                        values = new float[6] { 0,0,0,0,0,0 };
                        break;
                    }

                    values[counter] = float.Parse(valueAux)/* /(i>2?8192f:360*32.8f)*/;
                    counter++;
                    // Debug.Log("valor numero " + counter + "=" + valueAux);
                    valueAux = "";

                }
                else
                {
                    valueAux += s[i].ToString();
                }
            }
        }
        //  Debug.Log("\t" + values[0] + "\t |" + values[1] + "\t |" + values[2]  +"\t" + values[3]*360 + "\t |" + values[4]*360 + "\t |" + values[5]*360);
        values[3] *= 360;
        values[4] *= 360;
        values[5] *= 360;
        inputData = values;
    }

    private static void readFromArduinoThreaded(float timeOut)
    {

        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;
        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                lock (inputData)
                {
                    parseString(dataString);
                }
            }

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;


        } while (diff.Milliseconds < timeOut);

    }

    private static void requestLineFromArduino(float timeOut)
    {
        ThreadStart start = delegate { readFromArduinoThreaded(timeOut); };
        new Thread(start).Start();
    }

    private static void InputDataForShip()
    {
        lock (inputData)
        {
            pathGenerator.InputData = inputData;
            //Debug.Log(inputData[0] + "," + inputData[1] + "," + inputData[2] + "," + inputData[3]);
        }
    }
    public static void requestInputData()
    {
        requestLineFromArduino(0.01f);

        ThreadStart start = delegate { InputDataForShip(); };
        new Thread(start).Start();
    }

    public static void closePort()
    {
        if (stream != null)
        {
            Debug.Log("cierro");
            stream.Close();
            stream = null;
        }
    }



}
