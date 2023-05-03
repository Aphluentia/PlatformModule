import React, { useEffect, useState } from 'react';

function Module({ sessionData, webPlatformId, module}) {
  const [latestMessage, setLatestMessages] = useState(null);
  useEffect(() => {
    console.log(sessionData);
    const moduleSocket = new WebSocket(`ws://localhost:${module.serverPort}/${sessionData.sessionData.webPlatformId}`);
    
    moduleSocket.addEventListener('open', (event) => {
        console.log(module)
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
  }, [module, webPlatformId]);


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
