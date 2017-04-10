convert *.png -set filename:original %t -background none -gravity center -extent 537x537 %[filename:original].png
convert *.png -set filename:original %t -resize 256x256 %[filename:original].png
