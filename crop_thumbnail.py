#!/usr/bin/env python3
"""
crop_thumbnail.py
Crops and formats images to exactly 256x256 pixels for Thunderstore / R.E.P.O. mod icons.
"""

import sys
import argparse
from pathlib import Path
from PIL import Image


def crop_to_256(
    input_path: str,
    output_path: str = "icon.png",
    focus: str = "ducks"
) -> Path:
    src = Path(input_path)
    if not src.exists():
        raise FileNotFoundError(f"Source image not found: {src}")

    img = Image.open(src)
    width, height = img.size
    print(f"Source image: {src.name} ({width}x{height} px, {img.mode})")

    # If already height 256, perform horizontal crop
    if height == 256 and width >= 256:
        if focus == "center":
            left = (width - 256) // 2
        elif focus == "ducks":
            # Focus on the rubber ducks (shifted towards x ~ 270..526)
            left = min(max(270, 0), width - 256)
        else:
            left = (width - 256) // 2

        right = left + 256
        top = 0
        bottom = 256
        cropped = img.crop((left, top, right, bottom))
    else:
        # General case: scale proportionally so minimum dimension is 256, then center crop
        scale = 256 / min(width, height)
        new_w = int(round(width * scale))
        new_h = int(round(height * scale))
        resized = img.resize((new_w, new_h), Image.Resampling.LANCZOS)

        left = (new_w - 256) // 2
        top = (new_h - 256) // 2
        cropped = resized.crop((left, top, left + 256, top + 256))

    out = Path(output_path)
    out.parent.mkdir(parents=True, exist_ok=True)

    # Save as PNG
    cropped.save(out, format="PNG", optimize=True)
    file_size_kb = out.stat().st_size / 1024

    print(f"Saved: {out.resolve()} ({cropped.size[0]}x{cropped.size[1]} px, {file_size_kb:.1f} KB)")
    return out


def main():
    parser = argparse.ArgumentParser(description="Crop image to 256x256 Thunderstore icon")
    parser.add_argument("input", nargs="?", default=None, help="Input image path")
    parser.add_argument("-o", "--output", default="icon.png", help="Output path (default: icon.png)")
    parser.add_argument(
        "-f", "--focus",
        choices=["ducks", "center"],
        default="ducks",
        help="Focus area when cropping (default: ducks)"
    )

    args = parser.parse_args()

    # Default fallback to the latest uploaded user screenshot if not provided
    default_input = r"C:/Users/PRIDE CERBERO/.gemini/antigravity/brain/10312ad1-bd3d-4441-8dbb-edd9c1f18c4c/.user_uploaded/media_1790393118089.png"
    input_file = args.input or default_input

    crop_to_256(input_file, args.output, args.focus)


if __name__ == "__main__":
    main()
