using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class CanvasPanel : LayoutControl
    {
        protected override Vector2 MeasureOverride()
        {
            return Vector2.Zero;
        }
    }
}
