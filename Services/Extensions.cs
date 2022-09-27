using System;
using Nethereum.Hex.HexConvertors.Extensions;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;

namespace metamask_mini_api.Services;

public static class Extensions
{
    public static string ToHexString(this int number)
    {
        return "0x" + number.ToString("X2");
    }
    public static string ToHexString(this long number)
    {
        return "0x" + number.ToString("X2");
    }
    public static string ToHexString(this BigInteger number)
    {
        return "0x" + number.ToString("X2");
    }
    public static string ToHexString(this byte[] bytes)
    {
        return "0x" + Convert.ToHexString(bytes);
    }
    public static BigInteger HexStringToBigInt(this string hexNumber)
    {
        var input = hexNumber.StartsWith("0x")? hexNumber.Substring(2) : hexNumber;
        if (BigInteger.TryParse(input,
                    NumberStyles.AllowHexSpecifier,
                    null, out var result))
        {
            return result;
        }
        throw new Shared.JsonRpcErrorException(new Shared.JsonRpcError(
            Shared.ErrorType.InvalidRequest, "Unable to parse hexNumber " + hexNumber
        ));
    }
}