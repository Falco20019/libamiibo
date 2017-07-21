convert *.png -set filename:original %t -background none -gravity center -extent 408x408 %[filename:original].png
convert *.png -set filename:original %t -resize 256x256 %[filename:original].png
