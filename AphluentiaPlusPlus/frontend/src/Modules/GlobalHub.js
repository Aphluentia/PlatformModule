import React, { useEffect, useState } from 'react';
import Module from './Module.js';

function GlobalHub({ socketPort, webPlatformId }) {
  const [connections, setConnections] = useState({});

  useEffect(() => {
    const socket = new WebSocket(`ws://localhost:${socketPort}`);
    socket.addEventListener('open', (event) => {
      console.log('Socket connection established');
      socket.send(`{"Action":"CREATE_CONNECTION","PlatformId":"${webPlatformId}"}`);
    });

    socket.addEventListener('close', (event) => {
      console.log('Socket connection closed');
    });

    socket.addEventListener('error', (event) => {
      console.log('Socket error occurred:', event);
    });

    socket.addEventListener('message', (event) => {
      const message = JSON.parse(event.data);
      if (message.action === 'CREATE_CONNECTION') {
        if (!(message.data in connections)) {
          setConnections((prevState) => ({
            ...prevState,
            [message.data]: message,
          }));
        }
      } else if (message.action === 'CLOSE_CONNECTION') {
        // Handle CLOSE_CONNECTION action
      } else if (event.data.includes('NOT_AVAILABLE')) {
        // Handle NOT_AVAILABLE event
      }
    });

  }, [socketPort, webPlatformId]);

  return (
    <div className="message-list">
      {Object.keys(connections).length > 0 ? (
        Object.keys(connections).map((key, index) => (
          <Module moduleInformation={connections[key]} webPlatformId={webPlatformId} key={index} />
        ))
      ) : (
        <div>Not Connected</div>
      )}
    </div>
  );
}

export default GlobalHub;