﻿import React, { useContext } from 'react';
import {Page} from "../Page/Page";
import {fetchPosts, ListResponse, Post} from "../../Api/apiClient";
import {PostCard} from "../../Components/PostCard/PostCard";
import {InfiniteList} from "../../Components/InfititeList/InfiniteList";
import {Link} from "react-router-dom";
import './Feed.scss';
import { LoginContext } from '../../Components/LoginManager/LoginManager';

export function Feed(): JSX.Element {
    const {username, password} = useContext(LoginContext);
    const fetchPostsForList = async (page: number, pageSize: number): Promise<ListResponse<Post>> => {
        return fetchPosts(page, pageSize, username, password)
    } 
    return (
        <Page containerClassName={"feed"}>
            <h1 className="title">Feed</h1>
            <InfiniteList fetchItems={fetchPostsForList} renderItem={post => <PostCard key={post.id} post={post}/>}/>
            <Link className="create-post" to="/new-post">+</Link>
        </Page>
    );
}