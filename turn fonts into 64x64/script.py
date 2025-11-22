from PIL import Image, ImageDraw, ImageFont
import os

FONT_FILE = "pixelmix_bold.ttf"       # ← change to your .ttf or .otf file
OUTPUT_DIR = "glyphs_64"
BOX_SIZE = 64

os.makedirs(OUTPUT_DIR, exist_ok=True)

# Characters to generate: ASCII printable (32–126)
CHARS = [chr(i) for i in range(32, 127)]

def render_scaled_glyph(char, font_path, box_size):
    # Start with a large font size to measure maximum glyph area
    test_size = box_size * 4
    font = ImageFont.truetype(font_path, test_size)

    # Make a large canvas to draw test glyph
    img = Image.new("L", (test_size * 2, test_size * 2), 0)
    draw = ImageDraw.Draw(img)

    # Draw at large test scale
    draw.text((0, 0), char, fill=255, font=font)

    # Autocrop to get bounding box of the drawn glyph
    bbox = img.getbbox()

    if bbox is None:
        # Empty glyph (space, etc.)
        return Image.new("RGBA", (box_size, box_size), (0, 0, 0, 0))

    glyph = img.crop(bbox)
    gw, gh = glyph.size

    # Calculate scale to fit max dimension into 64x64
    scale = min(box_size / gw, box_size / gh)

    new_w = int(gw * scale)
    new_h = int(gh * scale)

    # Resize glyph
    glyph = glyph.resize((new_w, new_h), Image.LANCZOS)

    # Create final 64x64 canvas
    final = Image.new("RGBA", (box_size, box_size), (0, 0, 0, 0))

    # Center the glyph in the box
    x = (box_size - new_w) // 2
    y = (box_size - new_h) // 2

    final.paste(glyph, (x, y), glyph)

    return final


# Generate files
for i, ch in enumerate(CHARS):
    img = render_scaled_glyph(ch, FONT_FILE, BOX_SIZE)
    code = ord(ch)
    out_path = f"{OUTPUT_DIR}/{code}.png"
    img.save(out_path)
    print(f"Saved {out_path}")

print("Done!")
