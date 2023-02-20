const socket = new WebSocket('ws://localhost:9005');

socket.addEventListener('open', (event) => {
  console.log('WebSocket connection opened');
});

socket.addEventListener('message', (event) => {
  console.log('Received message:', event.data);
});

socket.addEventListener('close', (event) => {
  console.log('WebSocket connection closed');
});

socket.addEventListener('error', (event) => {
  console.log('WebSocket connection error:', event.error);
});
