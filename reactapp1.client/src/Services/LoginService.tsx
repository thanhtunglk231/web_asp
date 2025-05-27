/* eslint-disable @typescript-eslint/no-unused-vars */
import axios from 'axios';

export async function login(username:string , password : string) {
    try {
        const response = await axios.post("https://localhost:7089/api/Auth/Login", {
            username: username,
            password: password
        }, {
            headers: {
                'Content-Type': 'application/json'
            }
        });

        console.log(response.data);
    } catch (error) {
        if (error) {
            console.error("L?i phía server:", error);
        } else {
            console.error("L?i k?t n?i:", error);
        }
    }
}
