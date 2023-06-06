# PlatformModule
Contains the Aphluentia++ Web App 

# Run the Bridge Module:  
- Dockerfile:
    - docker build . -t bridge  
	- docker run --name BridgeModule -p 8010:80 -p 9010:443 -d bridge
### Requirements
    - nodejs
    - react-scripts
    - docker run --name redis -p 6379:6379 -d redis