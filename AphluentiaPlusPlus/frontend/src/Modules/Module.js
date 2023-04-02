import React, { useEffect, useState } from 'react';

function Module({ moduleInformation , webPlatformId}) {
  const [latestMessage, setLatestMessages] = useState(null);

  useEffect(() => {
    const moduleSocket = new WebSocket('ws://localhost:'+moduleInformation.data);
    
    moduleSocket.addEventListener('open', (event) => {
        console.log("Connecting to Module")
        console.log(moduleInformation)
        moduleSocket.send('{"Action":"CREATE_CONNECTION","PlatformId":"'+webPlatformId+'", "ModuleId":"'+moduleInformation.moduleId+'"}');
      });
    
      moduleSocket.addEventListener('close', (event) => {
        console.log('Socket connection closed');
      });
    
      moduleSocket.addEventListener('error', (event) => {
        console.log('Socket error occurred:', event);
      });
      moduleSocket.onmessage = function (evt) {
        let message = JSON.parse(evt.data);
        console.log(message);
        setLatestMessages(message);
      };
  }, [moduleInformation, webPlatformId]);


  return (
    <div className="message-list">
      { latestMessage ? (
        <p>
          {latestMessage ? JSON.stringify({latestMessage}): <p>Waiting for Messages</p>}
        </p>
      ) : (
        <div>Waiting For Messages</div>
      )}
    </div>
  );
}

export default Module;
