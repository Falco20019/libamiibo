convert MSS_amiibo_Baseball_DiddyKong.png -set filename:original %t -background none -gravity center -extent 256x256 %[filename:original].png
convert * -set filename:original %t -resize 256x256 %[filename:original].png


convert MSS_amiibo_Baseball_DiddyKong.png -resize 256x256 test.png