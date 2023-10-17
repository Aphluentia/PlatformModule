import React from 'react';

const ApplicationCard = ({ application, onRedirect }) => {
  return (
    <div>
      <h3>{application.name}</h3>
      <p>Description: {application.description}</p>
      <button onClick={onRedirect}>View Details</button>
    </div>
  );
};

export default ApplicationCard;