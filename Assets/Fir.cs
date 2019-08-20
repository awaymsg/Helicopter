using System.Collections;
using System.Collections.Generic;

public class Fir
{
    private float[] samples;
    private int write;
    private float divider;

    public Fir(int size)
    {
        write = 0;
        samples = new float[size];
        divider = 1f / size;
    }

    public void writeSample(float sample)
    {
        samples[write++] = sample;
        if (write == samples.Length)
        {
            write = 0;
        }
    }

    public float getOutput()
    {
        float accum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            accum += samples[i];
        }

        return accum * divider;
    }
}
