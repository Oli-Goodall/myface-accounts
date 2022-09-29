import React, {createContext, ReactNode, useState} from "react";

interface LoginContextType {
    username: string;
    password: string;
    isLoggedIn: boolean;
    isAdmin: boolean;
    logIn: (username: string, password: string) => void;
    logOut: () => void;
};

export const LoginContext = createContext<LoginContextType>({
    username: "",   
    password: "",
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(true);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    function logIn(username: string, password: string) {
        setUsername(username);
        setPassword(password);
        setLoggedIn(true);
    }
    
    function logOut() {
        setUsername("");
        setPassword("");
        setLoggedIn(false);
    }
    
    const context = {
        username: username,
        password: password,
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}