import React from 'react';
import {useContext, useState, useEffect} from 'react';
import './UserCard.css';
import isBase64 from 'is-base64';
const UserCard = ({user, userType, handleReject, handleAccept}) => {
  
    useEffect(() => {
    }, []);
    return (<>
         <div className="user-card">
            {user.profilePicture!=undefined && isBase64(user.profilePicture.split(",")[1]) ? (
                <img className="user-image"
                    src={user.profilePicture}
                    alt="Profile"
                />
            ):(
                <p>Image Invalid</p>
            )
            }
            <div className="user-info">
                <p><h3>{user.firstName} {user.lastName}</h3></p>
                <p>Email: {user.email}</p>
                <p>Phone: {user.phoneNumber}</p>
                <p>Address: {user.address}</p>
                <p>Age: {user.age}</p>
                {userType == 0 ? 
                <>    
                    <p>Credentials: {user.credentials}</p>
                    <p>Description: {user.description}</p>
                </>
                :
                <>  
                    <p>Condition: {user.conditionName}</p>
                    <p>Acquisition Date: {new Date(user.conditionAcquisitionDate).toLocaleDateString()}</p>
                </>
                }
            </div>
            <div className="user-actions">
                {handleReject != undefined ? <button onClick={() => handleReject(user.email)}>Delete</button>: <></> }
                {handleAccept != undefined ? <button onClick={() => handleAccept(user.email)}>Add</button>: <></> }
            </div>
        </div>
      </>
      
    );
  };
  
  export default UserCard;
  