using Microsoft.AspNetCore.Mvc;

namespace CassetteBeastsAPI.Models
{
    class CBRandom
    {
        ulong seed_value = 0;
        int global_seed = 0;
        ulong Modulus = 2147483647;
        ulong Multiplier = 16807;
        ulong Increment = 0;

        public CBRandom(ulong init_seed) {
            if (init_seed <= 0)
            {
                seed_value += Modulus - 1;
            }
            else
            {
                seed_value = init_seed;
            }
        }
        public ulong RandInt(int? maxVal = null)
        {
            if (maxVal != null && maxVal <= 0)
            {
                return 0;
            }

            
            seed_value = LCG(seed_value);

            ulong result = seed_value - 1;
            if (maxVal != null)
            {
                ulong castMaxVal = (ulong)(int)maxVal;
                result %= castMaxVal;
            }

            
            Console.WriteLine("seed: " + seed_value);
            Console.WriteLine("result: " + result);
            return result;
        }

        public ulong LCG(ulong seed_val)
        {
            ulong res = (Multiplier * seed_val + Increment) % Modulus;
            return res;
        }

        public int Choice(List<string> options)
        {
            return (int)RandInt(options.Count);
        }
    }
    
}
