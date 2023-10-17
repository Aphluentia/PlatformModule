import React, { useState, useEffect } from 'react';
import TherapistProfile from './TherapistProfile';
import PatientList from './PatientList';
import PendingPatientsList from './PendingPatientsList';
import PatientRequests from './PatientRequests';
import axios from 'axios';
import PatientAccepted from './PatientAccepted';
import './TherapistPage.css';

const TherapistPage = () => {
  const [AcceptedPatients, setAcceptedPatients] = useState([]);
  const [RequestedPatients, setRequestedPatients] = useState([]);
  const [AvailablePatients, setAvailablePatients] = useState([]);
  const [PendingPatients, setPendingPatients] = useState([]);
  const token = localStorage.getItem('Authentication');

  useEffect(() => {
    const email = localStorage.getItem('Email');
    const intervalId = setInterval(() => {
      getTherapistPatients();
    }, 1000); // Run every 1000 milliseconds (1 second)

    // Clean up the interval when the component unmounts
    return () => clearInterval(intervalId);
  }, []);
  const getTherapistPatients = () =>{
    axios
    .get("https://localhost:7176/api/Therapist/"+ token+"/Patients")
    .then(data =>{
        setAcceptedPatients(data.data.data.accepted);
        setRequestedPatients(data.data.data.requested);
        setAvailablePatients(data.data.data.available);
        setPendingPatients(data.data.data.pending);
        navigate("/Dashboard");
      

    } )
    .catch(error => {
    });

  }

  const handlePatientRequest = (email) => {
    console.log(email);
    axios
    .get("https://localhost:7176/api/Therapist/" + token + "/" + email)
    .then(data => {
      getTherapistPatients();
    })
    .catch(error => {
      // Handle error
    });
  };
  const handlePatientReject = (email) => {
    axios
    .delete("https://localhost:7176/api/Therapist/" + token + "/" + email)
    .then(data => {
      
      getTherapistPatients();
      console.log(JSON.stringify(data));
    })
    .catch(error => {
      // Handle error
    });
  };
 

  return (
    <div>
     {AcceptedPatients == null ||RequestedPatients == null ||AvailablePatients == null ||PendingPatients == null   ? (
                  null
          ) : (
            <div className="container">
              <div className="content-container">
                <TherapistProfile className="therapist-profile" />
                <PatientRequests className="patient-requests" patients={RequestedPatients} handlePatientReject = {handlePatientReject} handlePatientRequest = {handlePatientRequest}/>
                <PendingPatientsList className="patient-pending" patients={PendingPatients} handlePatientReject = {handlePatientReject}/>
              </div>
          
            <hr />
          
            <div className="content-container">
              <div className="two-thirds">
                <PatientAccepted patients={AcceptedPatients} handlePatientReject = {handlePatientReject} />
              </div>
          
             
            </div>
          
            <PatientList className="patient-list" patients={AvailablePatients} handlePatientRequest={handlePatientRequest} />
          </div>
          )}
                      
    </div>
  );
};

export default TherapistPage;
