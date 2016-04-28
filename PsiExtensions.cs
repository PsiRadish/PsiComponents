/*
 * User: Radish
 * Date: 10/21/2010
 * Time: 6:59 PM
 * Created in SharpDevelop
 */
using System;
using System.Drawing;

namespace PsiComponents.Extensions
{
    /// <summary>
    /// So far, just a wrapper for one annoying function of System.Drawing.Color.
    /// </summary>
    static class PsiExtensions
    {
        /*/ <summary>
        /// Addresses the absurdity of the single *signed* int version of FromArgb
        /// </summary>
        /// <param name="Argb">Proper unsigned int for passing a hex representation of any ARGB color value you please.</param>
        /// <returns>The Color structure that this method creates.</returns>
        public static Color FromArgb(this Color color, uint Argb)
        {
            return Color.FromArgb(unchecked ((int)Argb));
        }*/
    }
}

public static bool IsNumber(this object value)
{
    return (value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double);
}
