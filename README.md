# msite2

cd D:\SourceCode\Olympus\msite2
docker build -t msite2:1.0 .

docker run -d -p 8095:80 -p 5000:5000 --name msite2 msite2:1.0


